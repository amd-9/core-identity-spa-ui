import { useState } from 'react';
import axios from 'axios';
import './App.css';

const returnUrl = 'http://localhost:8080';

const App = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleAuthenticate = async () => {
    console.log('Authenticating!');
    const response = await axios.post('https://localhost:5443/api/authenticate', {
      username,
      password,
      returnUrl,
    });

    console.log('Resonse was', response);
  };

  const handleUsernameChange = (event) => setUsername(event.target.value);
  const handlePasswordChange = (event) => setPassword(event.target.value);

  return (
    <div className="login-form">
      <h2>
        IdentityServer 4 React Login SPA
      </h2>
      <main className="form-inputs">
        <input type="text" name="username" onChange={handleUsernameChange} placeholder="User name" value={username} />
        <input type="password" name="password" onChange={handlePasswordChange} placeholder="Password" value={password} />
        <button type="submit" onClick={handleAuthenticate}>Login</button>
      </main>
    </div>
  );
};

export default App;
