import { Routes } from '@angular/router';
import { AddUserComponent } from './components/add-user/add-user';
import { DashboardComponent } from './components/dashboard/dashboard';
import { UserForm } from './components/user-form/user-form';
import { SearchUser } from './components/search-user/search-user';

export const routes: Routes = [
  { path: '', redirectTo: 'add', pathMatch: 'full' },
  { path: 'add', component: UserForm },
  { path: 'search', component: SearchUser }
];