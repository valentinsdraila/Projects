import { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import '../styles/CardHover.css';


const SearchPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const navigate = useNavigate();
  const productsPerPage = 20;
  const [brands, setBrands] = useState([]);
  const [categories, setCategories] = useState([]);

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
  
    const fetchBrands = async () => {
      try {
        const response = await fetch("https://localhost:7007/api/products/brands", {
          credentials: "include",
        });
        if (!response.ok) {
          throw new Error("Failed to fetch brands");
        }
        const data = await response.json();
        setBrands(data.brands);
      } catch (error) {
        console.error("Error fetching brands:", error);
        setBrands([]);
      }
    };

    const fetchCategories = async () => {
      try {
        const response = await fetch("https://localhost:7007/api/products/categories", {
          credentials: "include",
        });
        if (!response.ok) {
          throw new Error("Failed to fetch categories");
        }
        const data = await response.json();
        setCategories(data.categories);
      } catch (error) {
        console.error("Error fetching categories:", error);
        setCategories([]);
      }
    };

    fetchBrands();
    fetchCategories();
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

  const getStockIcon = (stock) => {
  if (stock > 10) return "bi-check-circle";
  if (stock > 0) return "bi-exclamation-triangle";
  return "bi-x-circle";
};
const renderStars = (rating, clickable = false, onClick = () => {}) => {
  const stars = [];
  for (let i = 1; i <= 5; i++) {
    let starClass = "bi-star";

    if (i <= Math.floor(rating)) {
      starClass = "bi-star-fill"; 
    } else if (i - rating <= 0.5) {
      starClass = "bi-star-half";
    }

    stars.push(
      <i
        key={i}
        className={`bi ${starClass} ${starClass !== "bi-star" ? "text-warning" : "text-secondary"} ${clickable ? "cursor-pointer" : ""}`}
        style={{ cursor: clickable ? "pointer" : "default", fontSize: "1.25rem" }}
        onClick={() => clickable && onClick(i)}
      />
    );
  }
  return stars;
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
          {categories.map((category) => (
            <option name={category} value={category}>{category}</option>
          ))}
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
      {brands.map((brandName) => (
        <option key={brandName} value={brandName}>
          {brandName}
        </option>
      ))}
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
                className="card h-100 productcard"
                onClick={() => navigate(`/products/${product.id}`)}
                style={{ cursor: "pointer" }}
              >
                <img
                  src={`https://localhost:7007/images/${product.image}`}
                  alt={product.name}
                  className="img-fluid"
                  style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                />
                <div className="card-body">
                  <h5 className="card-title">{product.name}</h5>
                  <p className="card-text">
                    <strong>${product.unitPrice?.toFixed(2)}</strong>
                  </p>
                  <p className={`fw-semibold d-flex align-items-center ${getStockClass(product.quantity)}`}>
                    <i class={`bi ${getStockIcon(product.quantity)} me-2`}></i>
                    {getStockMessage(product.quantity)}
                  </p>
                  <div className="mb-3 d-flex align-items-center">
                    <div>{renderStars(product.rating.averageRating)}</div>
                    <div className="ms-2">{product.rating.averageRating}</div>
                    <div className="ms-2">({product.rating.totalReviews} reviews)</div>
                  </div>
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
