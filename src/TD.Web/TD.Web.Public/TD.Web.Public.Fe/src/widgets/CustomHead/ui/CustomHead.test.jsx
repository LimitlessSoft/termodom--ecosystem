import { render } from '@testing-library/react';
import { CustomHead } from './CustomHead';

// Mock next-seo components
jest.mock('next-seo', () => ({
  NextSeo: ({ title, description, additionalMetaTags, additionalLinkTags }) => (
    <div data-testid="next-seo">
      <meta name="title" content={title} />
      <meta name="description" content={description} />
      {additionalMetaTags?.map((tag, index) => (
        tag.property ? (
          <meta key={index} property={tag.property} content={tag.content} />
        ) : (
          <meta key={index} name={tag.name} content={tag.content} />
        )
      ))}
      {additionalLinkTags?.map((tag, index) => (
        <link key={index} rel={tag.rel} href={tag.href} type={tag.type} sizes={tag.sizes} />
      ))}
    </div>
  ),
  ProductJsonLd: (props) => (
    <script data-testid="product-jsonld" type="application/ld+json">
      {JSON.stringify(props)}
    </script>
  ),
}));

describe('CustomHead', () => {
  describe('Favicon Configuration (Latest Commit)', () => {
    it('renders all three favicon link tags correctly', () => {
      const { container } = render(<CustomHead />);

      const links = container.querySelectorAll('link[rel*="icon"]');
      expect(links).toHaveLength(3);

      // Check favicon.ico
      const faviconIco = container.querySelector('link[href="/favicon.ico"]');
      expect(faviconIco).toBeInTheDocument();
      expect(faviconIco).toHaveAttribute('rel', 'icon');
      expect(faviconIco).toHaveAttribute('sizes', 'any');

      // Check SVG icon
      const faviconSvg = container.querySelector('link[href="/termodom_logo.svg"]');
      expect(faviconSvg).toBeInTheDocument();
      expect(faviconSvg).toHaveAttribute('rel', 'icon');
      expect(faviconSvg).toHaveAttribute('type', 'image/svg+xml');

      // Check apple-touch-icon
      const appleTouchIcon = container.querySelector('link[rel="apple-touch-icon"]');
      expect(appleTouchIcon).toBeInTheDocument();
      expect(appleTouchIcon).toHaveAttribute('href', '/Termodom_Logo.png');
    });
  });

  describe('Open Graph Meta Tags (Latest Commit)', () => {
    it('renders og:image with correct property attribute and PNG URL', () => {
      const { container } = render(<CustomHead />);

      const ogImage = container.querySelector('meta[property="og:image"]');
      expect(ogImage).toBeInTheDocument();
      expect(ogImage).toHaveAttribute('content', 'https://www.termodom.rs/Termodom_Logo.png');
    });

    it('renders og:image:width with correct dimensions (500)', () => {
      const { container } = render(<CustomHead />);

      const ogImageWidth = container.querySelector('meta[property="og:image:width"]');
      expect(ogImageWidth).toBeInTheDocument();
      expect(ogImageWidth).toHaveAttribute('content', '500');
    });

    it('renders og:image:height with correct dimensions (500)', () => {
      const { container } = render(<CustomHead />);

      const ogImageHeight = container.querySelector('meta[property="og:image:height"]');
      expect(ogImageHeight).toBeInTheDocument();
      expect(ogImageHeight).toHaveAttribute('content', '500');
    });

    it('renders og:image:type with image/png', () => {
      const { container } = render(<CustomHead />);

      const ogImageType = container.querySelector('meta[property="og:image:type"]');
      expect(ogImageType).toBeInTheDocument();
      expect(ogImageType).toHaveAttribute('content', 'image/png');
    });

    it('renders og:site_name using default title when no title prop provided', () => {
      const { container } = render(<CustomHead />);

      const ogSiteName = container.querySelector('meta[property="og:site_name"]');
      expect(ogSiteName).toBeInTheDocument();
      expect(ogSiteName.getAttribute('content')).toContain('Termodom');
    });

    it('renders og:site_name using custom title when title prop provided', () => {
      const customTitle = 'Custom Page Title';
      const { container } = render(<CustomHead title={customTitle} />);

      const ogSiteName = container.querySelector('meta[property="og:site_name"]');
      expect(ogSiteName).toBeInTheDocument();
      expect(ogSiteName).toHaveAttribute('content', customTitle);
    });
  });

  describe('Additional Meta Tags', () => {
    it('renders viewport meta tag', () => {
      const { container } = render(<CustomHead />);

      const viewport = container.querySelector('meta[name="viewport"]');
      expect(viewport).toBeInTheDocument();
      expect(viewport).toHaveAttribute('content', 'width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0');
    });

    it('renders google-site-verification meta tag', () => {
      const { container } = render(<CustomHead />);

      const googleVerification = container.querySelector('meta[name="google-site-verification"]');
      expect(googleVerification).toBeInTheDocument();
    });

    it('renders keywords meta tag', () => {
      const { container } = render(<CustomHead />);

      const keywords = container.querySelector('meta[name="keywords"]');
      expect(keywords).toBeInTheDocument();
      expect(keywords).toHaveAttribute('content', 'gips ploče, fasade, suva gradnja, izolacija, cene');
    });
  });

  describe('Structured Data', () => {
    it('renders ProductJsonLd when structuredData is provided', () => {
      const structuredData = {
        name: 'Test Product',
        offers: {
          price: '1000',
          priceCurrency: 'RSD',
        },
      };

      const { getByTestId } = render(<CustomHead structuredData={structuredData} />);

      const productJsonLd = getByTestId('product-jsonld');
      expect(productJsonLd).toBeInTheDocument();
    });

    it('does not render ProductJsonLd when structuredData is not provided', () => {
      const { queryByTestId } = render(<CustomHead />);

      const productJsonLd = queryByTestId('product-jsonld');
      expect(productJsonLd).not.toBeInTheDocument();
    });

    it('adds product price meta tags when offers are provided as object', () => {
      const structuredData = {
        name: 'Test Product',
        offers: {
          price: '1500',
          priceCurrency: 'RSD',
        },
      };

      const { container } = render(<CustomHead structuredData={structuredData} />);

      const priceAmount = container.querySelector('meta[property="product:price:amount"]');
      const priceCurrency = container.querySelector('meta[property="product:price:currency"]');

      expect(priceAmount).toBeInTheDocument();
      expect(priceAmount).toHaveAttribute('content', '1500');
      expect(priceCurrency).toBeInTheDocument();
      expect(priceCurrency).toHaveAttribute('content', 'RSD');
    });

    it('adds product price meta tags when offers are provided as array', () => {
      const structuredData = {
        name: 'Test Product',
        offers: [
          {
            price: '2000',
            priceCurrency: 'RSD',
          },
        ],
      };

      const { container } = render(<CustomHead structuredData={structuredData} />);

      const priceAmount = container.querySelector('meta[property="product:price:amount"]');
      const priceCurrency = container.querySelector('meta[property="product:price:currency"]');

      expect(priceAmount).toBeInTheDocument();
      expect(priceAmount).toHaveAttribute('content', '2000');
      expect(priceCurrency).toBeInTheDocument();
      expect(priceCurrency).toHaveAttribute('content', 'RSD');
    });
  });

  describe('Title and Description', () => {
    it('uses custom title when provided', () => {
      const customTitle = 'Custom Page Title';
      const { container } = render(<CustomHead title={customTitle} />);

      const titleMeta = container.querySelector('meta[name="title"]');
      expect(titleMeta).toHaveAttribute('content', customTitle);
    });

    it('uses default title when not provided', () => {
      const { container } = render(<CustomHead />);

      const titleMeta = container.querySelector('meta[name="title"]');
      expect(titleMeta.getAttribute('content')).toContain('Termodom');
    });

    it('uses custom description when provided', () => {
      const customDescription = 'Custom page description';
      const { container } = render(<CustomHead description={customDescription} />);

      const descriptionMeta = container.querySelector('meta[name="description"]');
      expect(descriptionMeta).toHaveAttribute('content', customDescription);
    });

    it('uses default description when not provided', () => {
      const { container } = render(<CustomHead />);

      const descriptionMeta = container.querySelector('meta[name="description"]');
      expect(descriptionMeta.getAttribute('content')).toContain('Gipsane ploče');
    });
  });
});
