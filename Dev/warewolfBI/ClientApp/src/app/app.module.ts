import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CustomMaterialModule } from "./core/custommaterial.module";
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ModalModule } from 'angular-custom-modal';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AngMaterialModule } from './angmaterial';
import { LayoutModule } from '@angular/cdk/layout';
import { HttpModule } from "@angular/http";
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { ErrorInterceptor } from './helpers/error.interceptor';
import { AuthInterceptor } from './helpers/auth.interceptor';
import { GoogleChartsModule } from 'angular-google-charts';
import { sanitizeHtmlPipe } from './sanitize-html.pipe';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './core/app-routing.module';

import { ServerExplorerComponent } from './components/server-explorer/server-explorer.component';
import { OutputsExplorerComponent } from './components/outputs-explorer/outputs-explorer.component';
import { ExecutionloggingComponent } from './components/executionlogging/executionlogging.component';
import { SettingsComponent } from './components/settings/settings.component';


import { MediatorService } from './services/mediator.service';
import { UserService } from './services/user.service';
import { APIService } from './services/api.service';
import { ExecutionLoggingService } from './services/executionlogging.service';
import { ErrorDialogComponent } from './core/error-dialog/error-dialog.component';
import { LogEntryComponent } from './components/logentry/logentry.component';

@NgModule({
  entryComponents: [
    ErrorDialogComponent,
    LogEntryComponent
  ],
  declarations: [
    AppComponent,
    sanitizeHtmlPipe,
    ServerExplorerComponent,
    OutputsExplorerComponent,
    ExecutionloggingComponent,
    SettingsComponent
  ],
  imports: [
    NgbModule.forRoot(),
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpModule,
    CustomMaterialModule,
    AppRoutingModule,
    CommonModule,
    FlexLayoutModule,
    ModalModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    LayoutModule,
    GoogleChartsModule, MatDialogModule,MatInputModule 
  ],
  exports: [
    sanitizeHtmlPipe
  ],
  providers: [
    MediatorService,
    UserService,
    APIService,
    ExecutionLoggingService],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA,
    NO_ERRORS_SCHEMA
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
