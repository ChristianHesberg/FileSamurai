export async function post(route: string, body: object): Promise<Response> {
    try {
        return await fetch(`${process.env.API_BASE_URL}/${route}`, {
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
        return await fetch(`${process.env.API_BASE_URL}/${route}/${routeParams}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                //add jwt here
            }
        })
    } catch(error){
        console.log(error);
        throw error;
    }
}