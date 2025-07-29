import { Routes } from '@angular/router';
import { BatchserialComponent } from './batchserial/batchserial.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './login/login.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'batchserial', component: BatchserialComponent },
  { path: 'user', component: UserComponent },
];
