import 'core-js';
import 'reflect-metadata';
import 'zone.js/dist/zone';
import 'jquery';
import 'bootstrap-sass';

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './Modules/App/App.Module';

platformBrowserDynamic().bootstrapModule(AppModule);