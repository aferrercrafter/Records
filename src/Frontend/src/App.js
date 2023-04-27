import React, { useState } from 'react';
import './App.css';
import Register from './components/Register';
import Login from './components/Login';
import Records from './components/Records';

function App() {
  const [token, setToken] = useState(null);

  return (
    <div className="App">
      <header className="App-header">
        <h1>Ballast Lane</h1>
        {!token && (
          <>
            <Register />
            <Login setToken={setToken} />
          </>
        )}
        {token && <Records />}
      </header>
    </div>
  );
}

export default App;