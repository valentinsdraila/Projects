import { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";

const SearchPage = () => {
  const [searchParams] = useSearchParams();
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const query = searchParams.get("query");
  const [currentPage, setCurrentPage] = useState(1);
  const navigate = useNavigate();
  const productsPerPage = 20;

  useEffect(() => {
    if (!query) return;

    setLoading(true);
    fetch(`https://localhost:7007/api/products/search?query=${encodeURIComponent(query)}`, {
      method: "GET",
      credentials: "include"
    })
      .then(response => {
        if (!response.ok) throw new Error("Search failed");
        return response.json();
      })
      .then(data => setProducts(data))
      .catch(error => console.error(error))
      .finally(() => setLoading(false));
  }, [query]);

  const indexOfLastProduct = currentPage * productsPerPage;
  const indexOfFirstProduct = indexOfLastProduct - productsPerPage;
  const currentProducts = products.slice(indexOfFirstProduct, indexOfLastProduct);

    const getStockMessage = (stock) => {
  if (stock > 10) return "In stock";
  if (stock > 0) return "Stock running low";
  return "Out of stock";
};

const getStockClass = (stock) => {
  if (stock > 10) return "text-success";
  if (stock > 0) return "text-warning";
  return "text-danger";
};

  return (
    <div className="container mt-4">
      <h3>Search results for: <em>{query}</em></h3>
      {loading ? (
        <p>Loading...</p>
      ) : products.length === 0 ? (
        <p>No products found.</p>
      ) : (
        <div className="container mt-5">
      <div className="row mt-4">
        {currentProducts.map(product => (
          <div key={product.id} className="col-md-3 mb-4">
            <div 
              className="card h-100" 
              onClick={() => navigate(`/products/${product.id}`)} 
              style={{ cursor: "pointer" }}
            >
              <img 
                src={`https://localhost:7007/images/${product.image}`} 
                alt={product.name} 
                className="img-fluid" 
              />
              <div className="card-body">
                <h5 className="card-title">{product.name}</h5>
                <p className="card-text"><strong>${product.unitPrice?.toFixed(2)}</strong></p>
                 <p className={`fw-semibold ${getStockClass(product.quantity)}`}>
                    {getStockMessage(product.quantity)}
                </p>
              </div>
            </div>
          </div>
        ))}
      </div>

      {products.length > productsPerPage && (
        <nav>
          <ul className="pagination justify-content-center">
            {Array.from({ length: Math.ceil(products.length / productsPerPage) }, (_, i) => (
              <li key={i} className={`page-item ${currentPage === i + 1 ? "active" : ""}`}>
                <button 
                  className="page-link" 
                  onClick={() => setCurrentPage(i + 1)}
                >
                  {i + 1}
                </button>
              </li>
            ))}
          </ul>
        </nav>
      )}
    </div>
      )}
    </div>
  );
};

export default SearchPage;
