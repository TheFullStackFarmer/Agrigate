import type { Metadata } from "next";

import "@fontsource/inter";
import "./globals.css";
import { IconButton, Typography } from "@mui/joy";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars } from "@fortawesome/free-solid-svg-icons";
import AppBar from "@/components/app-bar/AppBar";

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
          <div className="bg-neutral-300 hidden md:block w-48 p-2 rounded-2xl">
            <Typography className="text-center">Agrigate</Typography>
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
