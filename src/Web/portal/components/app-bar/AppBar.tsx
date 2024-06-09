"use client";

import { useState } from "react";

import { faBars } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Drawer, IconButton, Typography } from "@mui/joy";

import Navigation from "@/components/navigation/Navigation";

export default function AppBar() {
  const [open, setOpen] = useState(false);

  return (
    <>
      <Typography fontSize="xl">Agrigate</Typography>
      <IconButton
        variant="plain"
        className="ml-auto"
        onClick={() => setOpen(true)}
      >
        <FontAwesomeIcon icon={faBars} />
      </IconButton>
      <Drawer
        open={open}
        onClose={() => setOpen(false)}
        slotProps={{
          content: {
            sx: {
              borderRadius: "0 1.5rem 1.5rem 0",
              padding: "1rem",
            },
          },
        }}
      >
        <Navigation />
      </Drawer>
    </>
  );
}
