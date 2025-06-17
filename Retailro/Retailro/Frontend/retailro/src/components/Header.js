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
            className="btn btn-link text-dark"
            onClick={() => navigate("/cart")}
            aria-label="Cart"
            title="Cart"
          >
            <i className="bi bi-cart3 fs-4"></i>
          </button>

          <button
            className="btn btn-link text-dark"
            onClick={() => navigate("/profile")}
            aria-label="Profile"
            title="Profile"
          >
            <i className="bi bi-person-circle fs-4"></i>
          </button>

          <button
            className="btn btn-link text-dark"
            onClick={() => {
              if (onLogout) onLogout();
              navigate("/login");
            }}
            aria-label="Logout"
            title="Logout"
          >
            <i className="bi bi-box-arrow-right fs-4"></i>
          </button>
        </div>
      </div>
    </header>
  );
};

export default Header;
