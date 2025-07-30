import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import { useAppDispatch } from '../../app/hooks';
import { AuthUser, setCredentials, setCurrentSchool, UserChild, UserSchool } from '../../app/authSlice';
import { useGetUserByIdQuery, useRegisterUserMutation } from '../users/api/apiSlice';
import { UserUpsertRequest } from '../users/api/apiSlice-generated';

const AuthRedirect = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const { instance } = useMsal();

  const [userId, setUserId] = useState<string | null>(null);
  const { data: dbUser, isLoading: loadingUser, error } = useGetUserByIdQuery(userId!, { skip: !userId });
  const [registerUser, { isLoading: registeringUser } ] = useRegisterUserMutation();

  // handle the redirect from MSAL and set the user id
  useEffect(() => {
    const handleRedirect = async () => {
      try {
        const response = await instance.handleRedirectPromise();
        
        if (response && response.account && response.accessToken) {
          console.log('Authentication successful:', response);
          instance.setActiveAccount(response.account);
          
          const idTokenClaims = response.account.idTokenClaims ?? {};
          const userOid = idTokenClaims['oid'];
          setUserId(userOid!);
        } else {
          navigate('/');
        }
      } catch (error) {
        console.error('Authentication redirect error:', error);
        navigate('/');
      }
    };

    handleRedirect();
  }, [navigate, instance]);

  // once the user id is set, fetch user data or create a new user
  useEffect(() => {
    if (!userId) return;

    const handleUserData = async () => {
      if (loadingUser) return; // still loading

      const account = instance.getActiveAccount();
      if (!account) throw new Error('No active account');

      const idTokenClaims = account.idTokenClaims ?? {};
      
      try {
        if (error) {
          console.log('User not found in database, registering new user...');

          const registerUserRequest: UserUpsertRequest = {
            id: userId,
            name: account.name ?? '',
            email: account.username
          };

          await registerUser(registerUserRequest).unwrap();

          const authUser: AuthUser = {
            id: registerUserRequest.id,
            name: registerUserRequest.name,
            email: registerUserRequest.email,
            roles: Array.isArray(idTokenClaims.roles)
              ? idTokenClaims.roles 
              : Array.isArray(idTokenClaims['extension_Roles']) ? idTokenClaims['extension_Roles'] : [],
            schools: [],
            children: []
          };

          const accessToken = instance.getActiveAccount()?.idToken;
          dispatch(setCredentials({ accessToken: accessToken || '', user: authUser }));
          
        } else if (dbUser) {
          console.log('User found in database:', dbUser);

          const userChildren: UserChild[] = dbUser.children.map(child => ({
            childId: child.childId,
            firstName: child.firstName,
            lastName: child.lastName,
            grade: child.grade,
            schoolId: child.schoolId,
            schoolName: child.schoolName
          }));

          const userSchools: UserSchool[] = dbUser.children.map(child => ({
            schoolId: child.schoolId,
            schoolName: child.schoolName
          }));

          const uniqueUserSchools: UserSchool[] = Array.from(
            new Map(userSchools.map(school => [school.schoolId, school])).values()
          );

          const authUser: AuthUser = {
            id: dbUser.id,
            email: dbUser.email,
            name: dbUser.name,
            roles: Array.isArray(idTokenClaims.roles)
              ? idTokenClaims.roles 
              : Array.isArray(idTokenClaims['extension_Roles']) ? idTokenClaims['extension_Roles'] : [],
            schools: uniqueUserSchools,
            children: userChildren
          };

          const accessToken = instance.getActiveAccount()?.idToken;
          dispatch(setCredentials({ accessToken: accessToken || '', user: authUser }));
          dispatch(setCurrentSchool(authUser.schools[0] ?? null));
        }

        setTimeout(() => navigate('/home'), 1000);

      } catch (err) {
        console.error('Error handling user data:', err);
        navigate('/');
      }
    };

    handleUserData();
  }, [userId, dbUser, loadingUser, error, instance, dispatch, navigate, registerUser]);

  return (
    <div style={{ 
      display: 'flex', 
      justifyContent: 'center', 
      alignItems: 'center', 
      height: '100vh',
      flexDirection: 'column'
    }}>
      <h2>Processing authentication...</h2>
      <p>Please wait while we complete your sign-in.</p>
      {registeringUser && <p>Registering new user...</p>}
      {loadingUser && <p>Loading user profile...</p>}
    </div>
  );
};

export default AuthRedirect;
