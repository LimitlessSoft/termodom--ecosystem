function generateRobots() {
  return `User-agent: *
  Allow: /
  
  Sitemap: https://termodom.rs/sitemap.xml
 `;
}

function Robots() {
  // getServerSideProps will do the heavy lifting
}

export async function getServerSideProps({ res }: any) {

  res.setHeader('Content-Type', 'text/plain');
  // we send the XML to the browser
  res.write(generateRobots());
  res.end();

  return {
    props: {},
  };
}

export default Robots;