import ReactDOM from 'react-dom/client';
import App from './App';
import { store } from './app/store';
import { Provider } from 'react-redux';
import { MsalProvider } from "@azure/msal-react";
import { msalInstance } from './msalConfig';
import * as serviceWorker from './serviceWorker';
import './main.css'; // Import global styles
import { StrictMode } from 'react';

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

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();