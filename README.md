# Example.GraphQL
HotChocolate API GraphQL with .NET8!
Docs: https://chillicream.com/docs/hotchocolate/v13/get-started-with-graphql-in-net-core

Strawberry Shake GraphQL client with .NET8!
Docs:
https://chillicream.com/docs/strawberryshake/v13/get-started

This link from **SingletonSean** is specific to firebase setup: 
https://www.youtube.com/watch?v=7Xk0BuisZjg&list=PLA8ZIAm2I03g9z705U3KWJjTv0Nccw9pj&index=14

FIREBASE: https://console.firebase.google.com/
Create the project and then register the user and login in:

FIREBASE REGISTER ENDPOINT: https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=[YOUR KEY]
```
{
 "email":"some.gmail.accouny@gmail.com",
 "password": "FooBar12345#@",
 "returnSecureToken": true
}
```
FIREBASE LOGIN ENDPOINT: https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=[YOUR KEY]
```
{
 "email":"some.gmail.accouny@gmail.com",
 "password": "FooBar12345#@",
 "returnSecureToken": true
}
```
My implementation is based on this tutorial series but with .net8 and has diffrent approaches:

**SingletonSean** -> Learn how to setup a GraphQL API in .NET using the *Hot Chocolate* package. 
https://www.youtube.com/playlist?list=PLA8ZIAm2I03g9z705U3KWJjTv0Nccw9pj

**SingletonSean** -> Learn how to setup a GraphQL client application in .NET using the *Strawberry Shake* package.
https://www.youtube.com/watch?v=6oJlibTwn_4&list=PLA8ZIAm2I03hQoVCdRzADYN6UBLnJNaSl

#### Example queries:
Banana Cake Pop queries:
https://chillicream.com/products/bananacakepop/

```
query {
  courses {
        id
        name
        instructor {
                id
                salary
                }        
    }
}

query {
  specificCourses {
            edges {
                node {
                    id
                    name
                    instructor {
                            id
                            salary
                            }
                }
            }
  }
}

query {
  courseById(id: "364e30e4-9f80-4325-a70e-b06fb0f5301a") {
    id
    name
    subject
    instructor {
            id
            salary
            lastName
            }
  }
}

mutation {
  createCourse(
    courseInput: {
      name: "dragan",
      subject: SCIENCE,
      instructorId: "364e30e4-9f80-4325-a70e-b06fb0f5301a"
    } )
    {
      id
      name
      subject
      instructorId
    }
}

mutation {
  updateCourse(
    id: "4d511f7b-9c6b-4b90-bd14-a6ae4dc1c559",
    courseInput:  {
        name: "dragan",
        subject: SCIENCE,
        instructorId: "364e30e4-9f80-4325-a70e-b06fb0f5301a"
    } )
    {
      id
      name
      subject
      instructorId
    }
}

subscription {
    courseCreated {
      id
      name
      subject
    }
}

subscription {
    courseUpdated(
      courseId: "4d511f7b-9c6b-4b90-bd14-a6ae4dc1c559"
      ) {
      id
      name
      subject
      instructorId
    }
}
```