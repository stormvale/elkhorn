import { useEffect, useState } from "react";
import { useMsal } from "@azure/msal-react";

interface School {
  id: string;
  name: string;
}

const Register = () => {
  const [schools, setSchools] = useState<School[]>([]);
  const [selectedSchoolId, setSelected] = useState<string>("");
  const [isLoading, setIsLoading] = useState(true);
  const { instance } = useMsal();

  useEffect(() => {
    const loadSchools = async () => {
      try {
        // In a real implementation, this would fetch available schools
        // based on the parent's location, invitation, or other criteria
        // const response = await fetch("/api/schools");
        // const schoolData = await response.json();
        setSchools([
          { id: "221", name: "Fairview Elementary" },
          { id: "291", name: "Rock City Elementary" },
          { id: "299", name: "Uplands Park Elementary" }
        ]);
      } catch (error) {
        console.error("Failed to load schools:", error);
      } finally {
        setIsLoading(false);
      }
    };

    loadSchools();
  }, []);

  const handleNext = async () => {
    if (!selectedSchoolId) {
      alert("Please select a school before continuing.");
      return;
    }

    try {
      // Store the selected school context
      sessionStorage.setItem("schoolId", selectedSchoolId);
      
      // Create a proper login request
      const signUpRequest = {
        scopes: ["openid", "profile", "email"],
        extraQueryParameters: {
          // You can pass the school context as a parameter
          school_context: selectedSchoolId
        },
        prompt: "select_account" // This ensures the user can choose or create an account
      };

      console.log("Initiating login with request:", signUpRequest);
      
      // Initiate Entra External ID sign-up flow using the MSAL React instance
      await instance.loginRedirect(signUpRequest);
    } catch (error) {
      console.error("Registration failed:", error);
      const errorMessage = error instanceof Error ? error.message : String(error);
      alert(`Registration failed: ${errorMessage}`);
    }
  };

  if (isLoading) {
    return <div>Loading schools...</div>;
  }

  return (
    <div style={{ padding: "2rem", maxWidth: "400px", margin: "0 auto" }}>
      <h2>Welcome to Elkhorn!</h2>
      <p>Select your school to get started. New users can create an account, and returning users can sign in with their existing account.</p>
      
      <div style={{ marginBottom: "1rem" }}>
        <label htmlFor="school-select" style={{ display: "block", marginBottom: "0.5rem" }}>
          School:
        </label>
        <select 
          id="school-select"
          value={selectedSchoolId}
          onChange={e => setSelected(e.target.value)}
          style={{ width: "100%", padding: "0.5rem", fontSize: "1rem" }}
        >
          <option value="">-- Please select a school --</option>
          {schools.map(school => (
            <option key={school.id} value={school.id}>
              {school.name}
            </option>
          ))}
        </select>
      </div>

      <button 
        onClick={handleNext}
        disabled={!selectedSchoolId}
        style={{ 
          width: "100%", 
          padding: "0.75rem", 
          fontSize: "1rem",
          backgroundColor: selectedSchoolId ? "#007acc" : "#ccc",
          color: "white",
          border: "none",
          borderRadius: "4px",
          cursor: selectedSchoolId ? "pointer" : "not-allowed"
        }}
      >
        Continue with Social Account
      </button>
      
      <p style={{ marginTop: "1rem", fontSize: "0.9rem", color: "#666" }}>
        Sign in or create an account using Google, Facebook, or Microsoft. 
        Returning users will see their existing accounts.
      </p>
    </div>
  );
}

export default Register;