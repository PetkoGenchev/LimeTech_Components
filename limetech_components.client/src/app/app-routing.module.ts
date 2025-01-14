import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BasketComponent } from './components/basket/basket.component';
import { HomeComponent } from './components/home/home.component';
import { AddComponentComponent } from './components/add-component/add-component.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'basket', component: BasketComponent },
  { path: 'add-component', component: AddComponentComponent }, 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
