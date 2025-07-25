 # Routing
 - Main route: /products — displays product listing.
 - Child route: /products/:id — displays product details based on the id (route parameter).
 - Protect both routes using a route guard (canActivate).
 #  API Integration
 - Use DummyJSON Products API
	- Example list API: https://dummyjson.com/products/search?q=
	- Example detail API: https://dummyjson.com/products/{id}
 # Search
 - Add a search bar to filter products by name.
 - Use debounce (500ms) to reduce API calls as the user types.
 # Infinite Scroll
 - Load more results as the user scrolls.
 - Support pagination using limit and skip parameters.
 # Product Card Display
 - Show product image, title, price, and description.
 - Use card layout with a responsive grid (Angular Material or CSS Grid).
 -  Loading & Empty States
 - Show a loader while fetching data.
 - Show a "No results found" message if search returns nothing.
 # Authentication Simulation
 - Store a mock token in localStorage on login.
 - AuthGuard should check the presence of this token to allow route access.
# Technical Requirements
 - Use Angular's HttpClient, ActivatedRoute, and Router
 - Use RxJS operators: debounceTime, switchMap, distinctUntilChanged
 - Implement a custom AuthGuard
 - Display error handling for invalid product IDs in detail route
