import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/auth.guard';
import { Routes, RouterModule } from '@angular/router';
import { MenuComponent } from './menu/menu.component';
import { PaymentComponent } from './payment/payment.component';
import { ListPaymentComponent } from './list-payment/list-payment.component';
import { PreviewDeleteComponent } from './preview-delete/preview-delete.component';
import { ChartPaymentComponent } from './chart-payment/chart-payment.component';

const appRoutes: Routes =
[
    { path: 'login', component: LoginComponent },
    { path: 'payment/new', component: PaymentComponent, canActivate: [AuthGuard] },
    { path: '', component: MenuComponent, canActivate: [AuthGuard] },
    { path: 'payment/edit', component: PaymentComponent, canActivate: [AuthGuard] },
    { path: 'payment/list', component: ListPaymentComponent, canActivate: [AuthGuard]  },
    { path: 'payment/delete', component: PreviewDeleteComponent, canActivate: [AuthGuard]  },
    { path: 'payment/grapth', component: ChartPaymentComponent, canActivate: [AuthGuard] },
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];
export const routing = RouterModule.forRoot(appRoutes);
