import {HttpError} from "./http.error";

export class UnauthorizedError extends HttpError {
    constructor(data?: unknown | undefined, message = 'Unauthorized access') {
        super(message, 401, data);
    }
}