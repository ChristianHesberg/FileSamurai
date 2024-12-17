export async function post(route: string, body: object): Promise<Response> {
    return fetch(`${process.env.API_BASE_URL}/${route}`, {
    method: 'POST',
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        //add jwt here
    },
    body: JSON.stringify(body)
    })
}

export async function get(route: string, routeParams: string): Promise<Response> {
    const response = await fetch(`${process.env.API_BASE_URL}/${route}/${routeParams}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            //add jwt here
        }
    })
    if (!response.ok) {
        throw new Error(`Response status: ${response.status}`);
    }
    return await response.json();
}