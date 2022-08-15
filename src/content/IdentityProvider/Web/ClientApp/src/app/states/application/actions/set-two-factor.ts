import { Metadata } from "../metadata";

export class SetTwoFactor {
	constructor(public isTwoFactorEnabled: boolean) { }
	static readonly type = `${Metadata.namespace}.SetTwoFactor`;
}
