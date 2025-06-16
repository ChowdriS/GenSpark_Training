import { Routes } from '@angular/router';
import { Home } from '../Components/home/home';
import { About } from '../Components/about/about';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: Home },
  { path: 'about', component: About },
  { path: '**', redirectTo: '/home' }
];