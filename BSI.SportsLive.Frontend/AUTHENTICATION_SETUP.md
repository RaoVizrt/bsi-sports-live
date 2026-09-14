# Authentication Setup Guide

This guide explains how to use the AuthService, Auth Interceptor, and Login component that have been created for JWT-based authentication.

## Files Created

### 1. **AuthService** (`src/app/services/auth.service.ts`)
Handles all authentication logic including:
- User login with credentials
- JWT token management (storing/retrieving/clearing)
- User state management
- Observable streams for reactive components

**Key Methods:**
- `login(credentials: LoginRequest)` - Login with username and password
- `logout()` - Clear token and user data
- `isAuthenticated()` - Check if user is logged in
- `getToken()` - Get the stored JWT token
- `refreshToken()` - Refresh the JWT token (optional)

**Observable Streams:**
- `currentUser$` - Current authenticated user
- `isAuthenticated$` - Authentication status

### 2. **Auth Interceptor** (`src/app/interceptors/auth.interceptor.ts`)
Automatically attaches the Bearer token to all API requests:
- Intercepts all HTTP requests
- Adds `Authorization: Bearer {token}` header
- Handles 401 Unauthorized errors by logging out the user

### 3. **Login Component** (`src/app/components/login/`)
A complete login UI with:
- Email/username input
- Password input with validation
- Form validation (required fields, minimum length)
- Loading state during login
- Error messages
- Beautiful styled UI with gradient background

**Files:**
- `login.component.ts` - Component logic
- `login.component.html` - Template
- `login.component.css` - Styling

### 4. **Auth Guard** (`src/app/services/auth.guard.ts`)
Protects routes that require authentication:
- Prevents unauthorized access
- Redirects to login page if not authenticated

### 5. **Updated Configuration** (`src/app/app.config.ts`)
- Added HttpClientModule
- Configured AuthInterceptor globally

## Usage

### Step 1: Update API Endpoint
In `src/app/services/auth.service.ts`, update the API URL:

```typescript
private apiUrl = 'api/auth'; // Change to your actual API endpoint
```

### Step 2: Add Routes
In `src/app/app.routes.ts`, add your protected routes with AuthGuard:

```typescript
{
  path: 'dashboard',
  component: DashboardComponent,
  canActivate: [AuthGuard]
}
```

### Step 3: Use Login Component
Add the login route (already configured) and navigate users to `/login`.

### Step 4: Access User in Components
In any component, inject AuthService to access user info:

```typescript
constructor(private authService: AuthService) {
  this.authService.currentUser$.subscribe(user => {
    console.log('Current user:', user);
  });
}
```

### Step 5: Logout
Call the logout method:

```typescript
this.authService.logout();
this.router.navigate(['/login']);
```

## API Requirements

Your backend API should:

1. **Login Endpoint** (`POST /api/auth/login`)
   - Accept: `{ username: string, password: string }`
   - Return: `{ access_token: string, token_type: string, expires_in?: number }`

2. **Refresh Token Endpoint** (optional) (`POST /api/auth/refresh`)
   - Return: `{ access_token: string, token_type: string, expires_in?: number }`

3. **Protected Endpoints**
   - Accept `Authorization: Bearer {token}` header
   - Return 401 if token is invalid/expired

## Example Login Request

```typescript
interface LoginRequest {
  username: string;
  password: string;
}

interface LoginResponse {
  access_token: string;
  token_type: string;
  expires_in?: number;
}
```

## Token Storage

Tokens are stored in `localStorage` with the key `access_token`:

```javascript
localStorage.getItem('access_token'); // Get token
localStorage.removeItem('access_token'); // Clear token
```

## Error Handling

The interceptor handles:
- **401 Errors**: Automatically logs out the user
- **Connection Errors**: Passed to the component for handling
- **Other Errors**: Passed to the component for handling

## Advanced Features

### Token Refresh
To implement automatic token refresh:

```typescript
this.authService.refreshToken().subscribe(
  response => console.log('Token refreshed'),
  error => console.log('Refresh failed, redirect to login')
);
```

### Custom Error Handling
Extend the interceptor to handle specific error codes:

```typescript
// In auth.interceptor.ts
if (error.status === 403) {
  // Handle forbidden access
}
```

## Security Notes

1. **HTTPS Only**: Always use HTTPS in production
2. **Secure Cookies**: Consider storing tokens in HTTP-only cookies instead of localStorage
3. **Token Expiration**: Implement token expiration checking
4. **CORS**: Configure CORS properly on your backend

## Customization

### Change Login Form Fields
Edit `login.component.ts` to add more form fields:

```typescript
this.loginForm = this.formBuilder.group({
  username: ['', [Validators.required]],
  password: ['', [Validators.required]],
  rememberMe: [false]
});
```

### Update Styling
Modify `login.component.css` to match your brand colors and design preferences.

### Custom Validation
Add RxJS operators to validate credentials in real-time:

```typescript
.pipe(
  debounceTime(300),
  distinctUntilChanged(),
  switchMap(username => this.authService.checkUsername(username))
)
```

## Testing

Example unit test for AuthService:

```typescript
it('should login user and store token', (done) => {
  const credentials = { username: 'test', password: 'pass' };
  authService.login(credentials).subscribe(response => {
    expect(authService.getToken()).toBeDefined();
    done();
  });
});
```

## Troubleshooting

### Token not being sent
- Check if token is stored in localStorage
- Verify API URL is correct
- Check browser DevTools Network tab for Authorization header

### 401 Errors after login
- Verify backend is returning valid JWT
- Check if token format is correct (should be Base64)
- Ensure server is validating token correctly

### CORS Errors
- Configure backend to accept requests from your frontend URL
- Add proper CORS headers to backend responses

---

For questions or issues, refer to Angular documentation:
- [Angular HttpClientModule](https://angular.io/guide/http)
- [Angular Guards](https://angular.io/guide/router#preventing-unauthorized-access)
- [Angular Interceptors](https://angular.io/guide/http-configure-server-communication#setting-up-a-custom-httpintercept)
