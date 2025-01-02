import {HttpError} from "./http.error";

export class ForbiddenError extends HttpError {
    constructor(data?: unknown | undefined, message = 'Forbidden') {
        super(message, 403, data);
    }
}