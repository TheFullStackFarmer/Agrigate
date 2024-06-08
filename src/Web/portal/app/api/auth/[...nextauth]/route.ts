import { jwtDecode, JwtPayload } from "jwt-decode";
import { Session } from "next-auth";
import { AdapterUser } from "next-auth/adapters";
import { JWT } from "next-auth/jwt";
import NextAuth from "next-auth/next";
import CredentialsProvider from "next-auth/providers/credentials";

interface AgrigateToken extends JwtPayload {
  name: string;
  givenname: string;
  surname: string;
  emailaddress: string;
  homephone: string;
  upn: string;
}

export interface AgrigateUser extends AdapterUser {
  phone: string;
  username: string;
  token: string;
}

interface AgrigateToken extends JWT {
  jwt: string;
}

interface AgrigateSession extends Session {
  token: string;
}

const handler = NextAuth({
  providers: [
    CredentialsProvider({
      name: "Username & Password",
      credentials: {
        username: { label: "Username", type: "text" },
        password: { label: "Password", type: "password" },
      },
      async authorize(credentials, req) {
        const response = await fetch(
          "http://localhost:5098/Authentication/Token",
          {
            method: "POST",
            body: JSON.stringify(credentials),
            headers: { "Content-Type": "application/json" },
          }
        );
        const payload = await response.json();
        const token = jwtDecode(payload.data.token) as AgrigateToken;

        const user = {
          id: token.sub ?? token.upn,
          name: token.name,
          email: token.emailaddress,
          phone: token.homephone,
          username: token.upn,
          token: payload.data.token,
        };

        if (user) {
          return user;
        }

        return null;
      },
    }),
  ],
  callbacks: {
    async jwt({ token, account, profile, user }) {
      const agrigateUser = user as AgrigateUser;
      const agrigateToken = token as AgrigateToken;

      if (agrigateUser && agrigateToken) agrigateToken.jwt = agrigateUser.token;

      return agrigateToken;
    },
    async session({ session, token, user }) {
      const agrigateToken = token as AgrigateToken;
      const agrigateSession = session as AgrigateSession;

      if (agrigateToken && agrigateSession) {
        const jwt = jwtDecode(agrigateToken.jwt) as AgrigateToken;
        const agrigateUser = agrigateSession.user as AgrigateUser;

        agrigateSession.token = agrigateToken.jwt;
        agrigateUser.username = jwt.upn;
        agrigateUser.phone = jwt.homephone;
        agrigateSession.user = agrigateUser;
      }

      return agrigateSession;
    },
  },
});

export { handler as GET, handler as POST };
