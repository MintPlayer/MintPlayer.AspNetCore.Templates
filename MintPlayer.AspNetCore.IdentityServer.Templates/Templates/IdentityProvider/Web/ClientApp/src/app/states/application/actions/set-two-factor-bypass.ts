import { Metadata } from "../metadata";

export class SetBypassTwoFactor {
    constructor(public bypassTwoFactorForExternallogin: boolean) {}
    static readonly type = `${Metadata.namespace}.SetBypassTwoFactor`;
}