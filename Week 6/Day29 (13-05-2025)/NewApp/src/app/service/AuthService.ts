import { Injectable } from '@angular/core';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private dummyUsers: User[] = [
    { username: 'admin', password: 'admin123' },
    { username: 'user', password: 'user123' }
  ];

  login(user: User): User | null {
    const found = this.dummyUsers.find(
      u => u.username === user.username && u.password === user.password
    );
    if (found) {
      sessionStorage.setItem('loggedInUser', JSON.stringify(found));
    //   localStorage.setItem('loggedInUser', JSON.stringify(found));
      return found;
    }
    return null;
  }

  getLoggedInUser(): User | null {
    const user = sessionStorage.getItem('loggedInUser');
    // const user = localStorage.getItem('loggedInUser');
    return user ? JSON.parse(user) : null;
  }

  logout() {
    sessionStorage.removeItem('loggedInUser');
  }
}
