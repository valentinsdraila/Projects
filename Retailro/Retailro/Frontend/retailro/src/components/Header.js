import { useNavigate } from 'react-router-dom';

const Header = ({ username, onLogout, onSearch }) => {
  const navigate = useNavigate();

  return (
    <header className="navbar navbar-expand-lg navbar-light bg-light px-3 py-2 shadow-sm">
      <div className="container-fluid d-flex justify-content-between align-items-center">
        <span
          className="navbar-brand mb-0 h1"
          style={{ cursor: 'pointer', fontWeight: 'bold' }}
          onClick={() => navigate('/home')}
        >
          RetailRo
        </span>

        <form 
          className="d-flex flex-grow-1 mx-4" 
          onSubmit={(e) => {
            e.preventDefault();
            const query = e.target.elements.search.value;
            onSearch(query);
          }}
        >
          <input
            type="search"
            name="search"
            className="form-control me-2"
            placeholder="Search products..."
            autoComplete='off'
            style={{ maxWidth: '800px', width: '100%', marginLeft:'15%'}}
          />
          <button className="btn btn-light" type="submit">
            <i className="bi bi-search"></i>
          </button>
        </form>

        <div className="dropdown">
          <button 
            className="btn btn-secondary dropdown-toggle" 
            type="button" 
            id="userDropdown" 
            data-bs-toggle="dropdown"
          >
            {username || "Guest"}
          </button>
          <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="userDropdown">
            <li><a className="dropdown-item" href="/cart">Shopping Cart</a></li>
            <li><a className="dropdown-item" href="/myorders">My Orders</a></li>
            <li><a className="dropdown-item" href="/profile">Profile</a></li>
            <li><button className="dropdown-item" onClick={onLogout}>Logout</button></li>
          </ul>
        </div>
      </div>
    </header>
  );
};

export default Header;
