import { useState } from "react";
import { endpoints } from "../../data/endpoints";
import "./AssistantForm.css";

function AssistantForm() {
    const [selectedEndpoint, setSelectedEndpoint] = useState("");
    const [prompt, setPrompt] = useState("");
    const [response, setResponse] = useState({
        loading: false,
        result: null,
        error: null,
    });

    async function handleSubmit(e) {
        e.preventDefault();

        setResponse({ loading: true, result: null, error: null });

        try {
            const res = await fetch(
                `https://localhost:7126/api/ai/${selectedEndpoint}`,
                {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ prompt }),
                },
            );

            const data = await res.json();
            setResponse({ loading: false, result: data.text, error: null });
        } catch (error) {
            setResponse({
                loading: false,
                result: null,
                error: "Något gick fel, försök igen om en stund.",
            });
        }
    }

    const current = endpoints.find((e) => e.value == selectedEndpoint);

    return (
        <div className="assistant">
            <form onSubmit={handleSubmit} className="assistant-form">
                <select
                    required
                    value={selectedEndpoint}
                    onChange={(e) => setSelectedEndpoint(e.target.value)}
                >
                    <option value="" disabled hidden>
                        Välj...
                    </option>
                    {endpoints.map((endpoint) => (
                        <option key={endpoint.value} value={endpoint.value}>
                            {endpoint.label}
                        </option>
                    ))}
                </select>
                <textarea
                    required
                    value={prompt}
                    onChange={(e) => setPrompt(e.target.value)}
                    placeholder={current?.placeholder}
                ></textarea>
                <button type="submit" disabled={response.loading}>
                    {response.loading ? "Tänker..." : "Skicka"}
                </button>
            </form>
            {response.result && (
                <section className="result-card">
                    <p className="result-text">{response.result}</p>
                    <button
                        type="button"
                        className="copy-button"
                        onClick={() =>
                            navigator.clipboard.writeText(response.result)
                        }
                    >
                        Kopiera
                    </button>
                </section>
            )}
            {response.error && <p className="error">{response.error}</p>}
        </div>
    );
}

export default AssistantForm;
