import {API_BASE_URL} from "../../constants";

export async function post(route: string, body: object): Promise<Response> {
    try {
        console.log(API_BASE_URL);
        return await fetch(`${API_BASE_URL}/${route}`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                //add jwt here
            },
            body: JSON.stringify(body)
        })
    } catch(error){
        console.log(error);
        throw error;
    }
}

export async function get(route: string, routeParams: string): Promise<Response> {
    try {
        return await fetch(`${API_BASE_URL}/${route}/${routeParams}`, {
            method: 'GET',
            headers: {
                'Accept': 'text/plain',
                //add jwt here
            }
        })
    } catch(error){
        console.log(error);
        throw error;
    }
}