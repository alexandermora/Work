import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, BrowserXhr } from '@angular/http';
import { DataTableModule } from 'angular2-datatable';
import { ChartsModule } from 'ng2-charts';
import { AppComponent } from './app.component';
import { MethodPay } from './_models/method-pay';
import { VisibleService } from './_services/visible.service';
import { GlobalVarService } from './_services/global-var.service';
import { WebServiceClientService } from './_services/web-service-client.service';
import { LoginComponent } from './components/login/login.component';
import { MenuComponent } from './components/menu/menu.component';
import { PaymentComponent } from './components/payment/payment.component';
import { ListPaymentComponent } from './components/list-payment/list-payment.component';
import { FooterPageComponent } from './components/footer-page/footer-page.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ListOptionComponent } from './components/list-option/list-option.component';
import { routing } from './components/app.routing';
import { AuthGuard } from './components/_guards/auth.guard';
import { AuthenticationService } from './_services/authentication.service';
import { DatePickerModule } from 'ng2-datepicker';
import { BroserXhrWithProgress, ProgressService } from './_services/progress.service';
import { PreviewDeleteComponent } from './components/preview-delete/preview-delete.component';
import { ChartPaymentComponent } from './components/chart-payment/chart-payment.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';

@NgModule({
    declarations:
    [
        AppComponent,
        LoginComponent,
        MenuComponent,
        PaymentComponent,
        ListPaymentComponent,
        FooterPageComponent,
        NavbarComponent,
        ListOptionComponent,
        PreviewDeleteComponent,
        ChartPaymentComponent,
        UserProfileComponent
    ],
    imports:
    [
        BrowserModule,
        FormsModule,
        HttpModule,
        routing,
        DatePickerModule,
        DataTableModule,
        ChartsModule
    ],
    providers:
    [
        AuthGuard,
        AuthenticationService,
        VisibleService,
        GlobalVarService,
        WebServiceClientService,
        MethodPay,
        { provide: BrowserXhr, useClass: BroserXhrWithProgress },
        ProgressService
    ],
    bootstrap:
    [
        AppComponent
    ]
})
export class AppModule { }
/*
{ path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'login', component: LoginComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', redirectTo: 'home' }


*/
