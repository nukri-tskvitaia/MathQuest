import './HomePage.css';

const HomePage = () => {
    return (
        <div className="homepage">
            <section className="homepage-header">
                <h1>Welcome to MathQuest!</h1>
                <p>Your ultimate destination for learning math through fun and competition.</p>
            </section>

            <article className="homepage-article">
                <section className="homepage-section">
                    <h2>Get Started</h2>
                    <p>Ready to embark on your MathQuest journey? click on <a href="/learn">Learn</a> now to start exploring! 🚀</p>
                </section>

                <section className="homepage-section">
                    <h2>About MathQuest</h2>
                    <p>
                        This site is designed for school students who want to learn math. Users can find friends, add them,
                        message them, and compete with each other to climb to the top of the leaderboard and determine who is
                        the most hardworking. Along with these features, they can learn math theories through visual examples.
                    </p>
                </section>

                <section className="homepage-section">
                    <h2>Features</h2>
                    <ul>
                        <li>Learn math through visual examples</li>
                        <li>Compete with friends to climb the leaderboard</li>
                        <li>Find and add friends</li>
                        <li>Message friends</li>
                        <li>Access various math quizzes and challenges</li>
                    </ul>
                </section>

                <section className="homepage-section">
                    <h2>Why MathQuest?</h2>
                    <blockquote>
                        &quot;Mathematics is not about numbers, equations, computations, or algorithms: it is about understanding.&quot; – William Paul Thurston
                    </blockquote>
                    <p>
                        At MathQuest, we believe in making math fun and engaging. Our platform offers a unique blend of learning and competition, ensuring students stay motivated and eager to improve their math skills.
                    </p>
                </section>
            </article>
        </div>
    );
};

export default HomePage;