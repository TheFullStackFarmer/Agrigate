"use client";

import { AgrigateUser } from "@/app/api/auth/[...nextauth]/route";
import {
  faRightFromBracket,
  faSquarePollVertical,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  Avatar,
  IconButton,
  List,
  ListItem,
  ListItemButton,
  ListItemContent,
  ListItemDecorator,
  Tooltip,
  Typography,
} from "@mui/joy";
import { signOut, useSession } from "next-auth/react";
import { usePathname } from "next/navigation";
import { useEffect, useState } from "react";

export default function Navigation() {
  const pathname = usePathname();
  const session = useSession();

  // Prevent hydration errors by waiting to render the initials & session
  // data until we're client-side
  const [isClient, setIsClient] = useState(false);

  useEffect(() => {
    setIsClient(true);
  }, []);

  const handleLogout = () => {
    signOut();
  };

  let username = "";
  let initials = "";
  if (isClient && session.data?.user) {
    username =
      session.data.user.name?.trim()?.length ?? 0 > 0
        ? (session.data.user.name as string)
        : (session?.data?.user as AgrigateUser)?.username;

    initials = username.match(/\b(\w)/g)?.join("") ?? "";
  }

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
      {isClient && (
        <ListItem>
          <ListItemDecorator>
            <Avatar size="lg">{initials}</Avatar>
          </ListItemDecorator>
          <ListItemContent sx={{ marginLeft: "1rem" }}>
            <Typography sx={{ textOverflow: "ellipsis", overflow: "hidden" }}>
              {username}
            </Typography>
            <Tooltip title={session.data?.user?.email}>
              <Typography
                fontSize="xs"
                sx={{ textOverflow: "ellipsis", overflow: "hidden" }}
              >
                {session.data?.user?.email}
              </Typography>
            </Tooltip>
          </ListItemContent>
          <IconButton onClick={handleLogout}>
            <FontAwesomeIcon icon={faRightFromBracket} />
          </IconButton>
        </ListItem>
      )}
    </div>
  );
}
