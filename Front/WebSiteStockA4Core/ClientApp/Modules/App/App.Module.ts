import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { HttpModule } from '@angular/http';

import { Configuration } from 'App.Config';

// Providers (Services)
//import { } from 'Services/API.Service';

import { TestComponent } from './Components/Test.Component';

// Components (views)
//import { } from '';
//import { } from '';
//import { } from '';
//import { } from '';

//App routes
const UrlRoutes: Routes = [
  //{path: 'Home', loadChildren: 'Modules/Home/Home.Module?chunkName=Home'},
];

@NgModule({
  imports: [
    BrowserModule,
    CommonModule,
    HttpModule,
    RouterModule.forRoot(UrlRoutes, { useHash: true })
  ],
  providers: [
    { provide: 'Configuration', useValue: Configuration },
    //APIService    
  ],
  declarations: [
    TestComponent
    //LayoutComponent,
    //HeaderComponent,
    //NavigationComponent,
    //FotterComponent
  ],
  bootstrap: [//LayoutComponent
  ]
})

export class AppModule { }