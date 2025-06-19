# Build a simple User Management module where users can be created, searched, and filtered using RxJS, Observables, and Angular validations.
# Features to Implement:
# User Form (Reactive Form)
 - Fields: Username, Email, Password, Confirm Password, Role
 - Validations:
 - Required fields
 - Email pattern
 - Password strength: min length, number, symbol
 - Confirm Password matches Password
 - Custom Validator: Username cannot include banned words (e.g., "admin", "root")
# Live Search with RxJS Debounce
 - A search bar to filter users from a dummy user list (you can use a static array)
 - Use:
  fromEvent + debounceTime + distinctUntilChanged + switchMap
  Filter based on username or role
# User List with Observable Source
 - Store users in a BehaviorSubject<User[]>
 - On form submission, push new user into the stream
 - List updates dynamically when a user is added