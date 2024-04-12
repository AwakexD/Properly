const domain = 'https://localhost:44319';


async function requester(method, endPoints, data, token) {
    const options = {
        method,
        headers: {
            'Content-Type': 'application/json',
        },
        body: data ? JSON.stringify(data) : undefined,
    };

    if (token) {
        options.headers['X-CSRF-TOKEN'] = token;
    }

    try {
        const response = await fetch(domain + endPoints, options);

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message);
        }

        if (response.status === 204) {
            return response;
        }

        const contentType = response.headers.get('content-type');
        if (contentType && contentType.includes('application/json')) {
            return await response.json();
        }
        return await response.text();

    } catch (error) {
        throw error;
    }
}

const get = requester.bind(null, 'GET');
const post = requester.bind(null, 'POST');
const put = requester.bind(null, 'PUT');
const patch = requester.bind(null, 'PATCH');
const del = requester.bind(null, 'DELETE');

export {
    get,
    post,
    put,
    patch,
    del as delete,
};