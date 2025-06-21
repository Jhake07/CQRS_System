import { Routes } from '@angular/router';
import { BatchserialComponent } from './batchserial/batchserial.component';
import { UserComponent } from './user/user.component';

export const routes: Routes = [
  { path: 'batchserial', component: BatchserialComponent },
  { path: 'user', component: UserComponent },
];
