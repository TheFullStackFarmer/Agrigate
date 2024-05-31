import type { Metadata } from "next";

import AppBar from "@/components/app-bar/AppBar";
import Navigation from "@/components/navigation/Navigation";

import "@fontsource/inter";
import "./globals.css";

export const metadata: Metadata = {
  title: "Agrigate",
  description: "An open source farm management platform",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en" className="h-full">
      <body className="bg-neutral-200 p-4 h-full flex flex-col gap-4">
        <div className="bg-neutral-300 flex items-center md:hidden p-4 rounded-2xl">
          <AppBar />
        </div>

        {/* Main Content */}
        <div className="flex flex-row flex-grow gap-4">
          {/* Navigation */}
          <div className="bg-neutral-300 hidden md:block w-60 p-2 rounded-2xl h-full">
            <Navigation />
          </div>

          {/* Page Content */}
          <div className="bg-neutral-300 flex-grow p-2 rounded-2xl">
            Content
          </div>
          {/* <div>{children}</div> */}
        </div>
      </body>
    </html>
  );
}
