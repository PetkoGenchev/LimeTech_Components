import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { BasketComponent } from './components/basket/basket.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { AddComponentComponent } from './components/add-component/add-component.component';
import { EditComponentComponent } from './components/edit-component/edit-component.component';
import { ManageComponentsComponent } from './components/manage-components/manage-components.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'basket', component: BasketComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'add', component: AddComponentComponent },
  { path: 'edit/:id', component: EditComponentComponent },
  { path: 'manage', component: ManageComponentsComponent },
];
