import { LoginResult } from "./login-result";

export interface ExternalLoginResult extends LoginResult {
	targetOrigin?: string;
	provider: string;
}
