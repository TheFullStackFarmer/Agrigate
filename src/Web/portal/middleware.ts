// import { NextRequest } from "next/server";

// export async function middleware(request: NextRequest) {
//   console.log("request:", request);
//   const currentUser = request.cookies.get("currentUser")?.value;

//   const loginPage = request.nextUrl.pathname.startsWith("/auth/login");
//   const registrationPage =
//     request.nextUrl.pathname.startsWith("/auth/register");

//   if (!currentUser && !loginPage && !registrationPage)
//     return Response.redirect(new URL("/auth/login", request.url));
// }

// export const config = {
//   matcher: ["/((?!api|_next/static|_next/image|.*\\.png$).*)"],
// };

export { default } from "next-auth/middleware";

// export const config = { matcher: ["/dashboard"] };
