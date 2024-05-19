import type { SidebarsConfig } from "@docusaurus/plugin-content-docs";

/**
 * Creating a sidebar enables you to:
 - create an ordered group of docs
 - render a sidebar for each doc of that group
 - provide next/previous navigation

 The sidebars can be generated from the filesystem, or explicitly defined here.

 Create as many sidebars as you want.
 */
const sidebars: SidebarsConfig = {
  // By default, Docusaurus generates a sidebar from the docs folder structure
  docs: [
    "intro",
    {
      type: "category",
      label: "Getting Started",
      link: {
        type: "generated-index",
      },
      items: ["getting-started/installation"],
    },
  ],
  design: [
    "design/intro",
    {
      type: "category",
      label: "Architecture",
      link: { type: "generated-index" },
      items: ["design/architecture/platform", "design/architecture/services"],
    },
    {
      type: "category",
      label: "Business Logic",
      link: { type: "generated-index" },
      items: ["design/business-logic/iot"],
    },
  ],
  // guides: [{ type: "autogenerated", dirName: "guides" }],
  // releases: [{ type: "autogenerated", dirName: "guides" }],

  // But you can create a sidebar manually
  /*
  tutorialSidebar: [
    'intro',
    'hello',
    {
      type: 'category',
      label: 'Tutorial',
      items: ['tutorial-basics/create-a-document'],
    },
  ],
   */
};

export default sidebars;
