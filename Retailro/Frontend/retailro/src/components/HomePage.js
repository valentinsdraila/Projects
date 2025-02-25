import { useEffect, useState } from "react";

const HomePage = () => {
  const [username, setUsername] = useState("");

  useEffect(() => {
    console.log("Fetching");
    fetch("https://localhost:7007/api/user/me", {
      method: "GET",
      credentials: "include" // Ensure cookies are sent with request
    })
      .then(response => {
        if (!response.ok) throw new Error("Failed to fetch username");
        return response.json();
      })
      .then(data => setUsername(data.username))
      .catch(error => console.error(error));
  }, []);

    return (
        <div className="container mt-5">
            <div className="d-flex justify-content-between align-items-center">
                <h2>Welcome, {username}!</h2>
                <div className="dropdown">
                    <button className="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false">
                        Options
                    </button>
                    <ul className="dropdown-menu" aria-labelledby="dropdownMenuButton">
                        <li><a className="dropdown-item" href="#">Profile</a></li>
                        <li><a className="dropdown-item" href="#">Orders</a></li>
                    </ul>
                </div>
            </div>
        </div>
    );
};

export default HomePage;
