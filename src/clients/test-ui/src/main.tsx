import ReactDOM from 'react-dom/client';
import { Provider } from 'react-redux';
import { StrictMode } from 'react';
import { MsalProvider } from '@azure/msal-react';
import App from './App';
import store from './app/store';
import { msalInstance } from './msalConfig';

// Initialize MSAL before rendering the app
msalInstance.initialize().then(() => {
  ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <StrictMode>
      <Provider store={store}>
        <MsalProvider instance={msalInstance}>
          <App />
        </MsalProvider>
      </Provider>
    </StrictMode>,
  );
}).catch((error) => {
  console.error('MSAL initialization failed:', error);
});
