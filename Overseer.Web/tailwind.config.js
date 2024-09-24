/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './**/*.html',
    './**/*.cshtml',
    './**/*.razor',
    './**/*.cs',
    './*.razor'
  ],
  theme: {
    extend: {},
  },
  plugins: [
    require('daisyui'),
  ],
  daisyui: {
    themes: [
      {
        light: {
          "primary": "#5475ff",
          "secondary": "#47ff98",
          "accent": "#fff64f",
          "neutral": "#d5d5d5",
          "base-100": "#e4e4e4",
          "info": "#44d8ff",
          "success": "#b4ff3e",
          "warning": "#ff8937",
          "error": "#fd3131",
        },
      },
      {
        dark: {
          "primary": "#5475ff",
          "secondary": "#47ff98",
          "accent": "#fff64f",
          "neutral": "#373737",
          "base-100": "#272727",
          "info": "#44d8ff",
          "success": "#b4ff3e",
          "warning": "#ff8937",
          "error": "#fd3131",
        },
      },
    ],
  },
}

