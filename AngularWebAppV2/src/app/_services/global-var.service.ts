import { Injectable } from '@angular/core';
import { MethodPay } from '../_models/method-pay';

@Injectable()
export class GlobalVarService
{
    public operation: string;
    public username: string;
    public timezone: string;
    // tslint:disable-next-line:no-inferrable-types
    public firstUpload: boolean = true;
    public fileName: string;
    public methodPay: MethodPay;
    public oldMethodPay: MethodPay;
    public firstTime: boolean;
    public typePaymentOriginal: string;
    // tslint:disable-next-line:no-inferrable-types
    public isUpdated: boolean = false;
    public dateSelected: any;
    public urlRoot: string = 'http://localhost:54251';
    constructor()
    {
    }
}
