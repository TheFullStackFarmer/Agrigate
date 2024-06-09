"use client";

import { usePathname } from "next/navigation";

import { faSquarePollVertical } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  List,
  ListItem,
  ListItemButton,
  ListItemContent,
  ListItemDecorator,
  Typography,
} from "@mui/joy";

import ProfileButton from "../profile-button/ProfileButton";

export default function Navigation() {
  const pathname = usePathname();

  return (
    <div className="flex flex-col h-full">
      <Typography className="text-center" fontSize="xl">
        Agrigate
      </Typography>
      <List>
        <ListItem>
          <ListItemButton selected={pathname === "/"} className="rounded-2xl">
            <ListItemDecorator>
              <FontAwesomeIcon icon={faSquarePollVertical} />
            </ListItemDecorator>
            <ListItemContent>
              <Typography>Dashboard</Typography>
            </ListItemContent>
          </ListItemButton>
        </ListItem>
      </List>
      <div className="grow" />
      <ProfileButton />
    </div>
  );
}
