import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { FileuploadPage } from './components/fileupload-page/fileupload-page';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' }, 
  { path: 'home', component: Home },
  { path: 'upload', component: FileuploadPage },
]
