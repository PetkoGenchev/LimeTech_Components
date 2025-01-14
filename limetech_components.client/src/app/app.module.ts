import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { BasketComponent } from './components/basket/basket.component';
import { HomeComponent } from './components/home/home.component';
import { AddComponentComponent } from './components/add-component/add-component.component';

@NgModule({
  declarations: [
    AppComponent,
    BasketComponent,
    HomeComponent,
    AddComponentComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
