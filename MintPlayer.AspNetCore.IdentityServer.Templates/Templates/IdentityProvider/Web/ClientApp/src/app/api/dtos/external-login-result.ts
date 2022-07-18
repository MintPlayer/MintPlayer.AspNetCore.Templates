import { LoginResult } from "./login-result";

export interface ExternalLoginResult extends LoginResult {
	provider: string;
}
