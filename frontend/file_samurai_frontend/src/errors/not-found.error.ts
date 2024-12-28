import {HttpError} from "./http.error";

export class NotFoundError extends HttpError {
    constructor(message = 'Resource not found') {
        super(message, 404);
    }
}