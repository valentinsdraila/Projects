import { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";

const SearchPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const navigate = useNavigate();
  const productsPerPage = 20;

  const query = searchParams.get("query") || "";
  const minPrice = searchParams.get("minPrice") || "";
  const maxPrice = searchParams.get("maxPrice") || "";
  const category = searchParams.get("category") || "";
  const brand = searchParams.get("brand") || "";
  const sort = searchParams.get("sort") || "relevance";

  const [tempMinPrice, setTempMinPrice] = useState(minPrice);
  const [tempMaxPrice, setTempMaxPrice] = useState(maxPrice);
  const [priceChanged, setPriceChanged] = useState(false);

  useEffect(() => {
    if (!query) return;

    setLoading(true);

    const params = new URLSearchParams({
      query,
      minPrice,
      maxPrice,
      category,
      brand,
      sort,
    });

    fetch(`https://localhost:7007/api/products/search?${params.toString()}`, {
      method: "GET",
      credentials: "include",
    })
      .then((response) => {
        if (!response.ok) throw new Error("Search failed");
        return response.json();
      })
      .then((data) => {
        setProducts(data.products || []);
        setCurrentPage(1);
      })
      .catch((error) => console.error(error))
      .finally(() => setLoading(false));
  }, [query, category, brand, minPrice, maxPrice, sort]);

  const indexOfLastProduct = currentPage * productsPerPage;
  const indexOfFirstProduct = indexOfLastProduct - productsPerPage;
  const currentProducts = products.slice(indexOfFirstProduct, indexOfLastProduct);

  const updateFilter = (key, value) => {
    const updatedParams = new URLSearchParams(searchParams);
    if (value) {
      updatedParams.set(key, value);
    } else {
      updatedParams.delete(key);
    }
    setSearchParams(updatedParams);
  };

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
      <div className="row mt-4">

  <div className="col-md-3" style={{marginLeft:"-5rem", maxWidth:"250px"}}>
    <div className="border p-3 rounded">
      <h5>Filters</h5>

      <div className="mb-3">
        <label className="form-label">Category</label>
        <select
          className="form-select"
          value={category}
          onChange={(e) => updateFilter("category", e.target.value)}
        >
          <option value="">All Categories</option>
          <option value="hoodies">Hoodies</option>
          <option value="shoes">Shoes</option>
        </select>
      </div>

      <div className="mb-3">
        <label className="form-label">Brand</label>
        <select
          className="form-select"
          value={brand}
          onChange={(e) => updateFilter("brand", e.target.value)}
        >
          <option value="">All Brands</option>
          <option value="Nike">Nike</option>
          <option value="Jordan">Jordan</option>
        </select>
      </div>

      <div className="mb-3">
        <label className="form-label">Min Price</label>
        <input
          type="number"
          className="form-control"
          value={tempMinPrice}
          onChange={(e) => {
            setTempMinPrice(e.target.value);
            setPriceChanged(true);
          }}
          placeholder="0"
        />
      </div>

      <div className="mb-3">
        <label className="form-label">Max Price</label>
        <input
          type="number"
          className="form-control"
          value={tempMaxPrice}
          onChange={(e) => {
            setTempMaxPrice(e.target.value);
            setPriceChanged(true);
          }}
          placeholder="1000"
        />
      </div>

      {priceChanged && (
        <div className="mb-3">
          <button
            className="btn btn-dark w-100"
            onClick={() => {
              const newParams = new URLSearchParams(searchParams);

              if (tempMinPrice) newParams.set("minPrice", tempMinPrice);
              else newParams.delete("minPrice");

              if (tempMaxPrice) newParams.set("maxPrice", tempMaxPrice);
              else newParams.delete("maxPrice");

              setSearchParams(newParams);
              setPriceChanged(false);
            }}
          >
            <i className="bi bi-check-circle me-2"></i>
          </button>
        </div>
      )}

      <div className="mb-3">
        <label className="form-label">Sort By</label>
        <select
          className="form-select"
          value={sort}
          onChange={(e) => updateFilter("sort", e.target.value)}
        >
          <option value="">Relevance</option>
          <option value="price_asc">Price: Low to High</option>
          <option value="price_desc">Price: High to Low</option>
          <option value="newest">Newest</option>
          <option value="nr_reviews">Number of Reviews</option>
        </select>
      </div>
    </div>
  </div>

  <div className="col-md-9">
          <h3>
        Search results for: <em>{query}</em>
      </h3>
    {loading ? (
      <p>Loading...</p>
    ) : products.length === 0 ? (
      <p>No products found.</p>
    ) : (
      <>
        <p>{products.length} results</p>
        <div className="row">
          {currentProducts.map((product) => (
            <div key={product.id} className="col-md-4 mb-4">
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
                  <p className="card-text">
                    <strong>${product.unitPrice?.toFixed(2)}</strong>
                  </p>
                  <p className={`fw-semibold ${getStockClass(product.quantity)}`}>
                    {getStockMessage(product.quantity)}
                  </p>
                  <p>{product.rating.averageRating}/5({product.rating.totalReviews})</p>
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
                  <button className="page-link" onClick={() => setCurrentPage(i + 1)}>
                    {i + 1}
                  </button>
                </li>
              ))}
            </ul>
          </nav>
        )}
      </>
    )}
  </div>
</div>

    </div>
  );
};

export default SearchPage;
